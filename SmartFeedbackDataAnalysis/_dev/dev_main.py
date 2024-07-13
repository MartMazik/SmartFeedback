import time

from mongoengine import connect

from _dev.config import MONGO_HOST, MONGO_DB
from models.text_group_model import GroupModel
from _dev.mongoEntities.group_text_objects import GroupTextObject
from _dev.mongoEntities.text_object import TextObject
from models.text_object_model import TextObjectModel
from services.grouping_service import group_text_objects
from services.preprocess_service import preprocess


def init_db():
    connect(db=MONGO_DB, host=MONGO_HOST)


def main():
    init_db()

    project_id = '65f8dec3b2371d94c7fbbef9'
    text_objects = TextObject.objects(project_id=project_id).all()
    groups = GroupTextObject.objects(project_id=project_id).all()

    print(f"Loaded {len(text_objects)} text objects, {len(groups)} groups.")

    group_models: list[GroupModel] = []
    for group in groups:
        center_text_object = TextObject.objects(id=group.center_text_object_id).first()
        center_text_object_model = TextObjectModel(
            id=str(center_text_object.id),
            content=center_text_object.content,
            processed_content=center_text_object.processed_content,
            project_id=str(center_text_object.project_id),
            analog_count=0,
            user_rating_count=0
        )
        text_object_models = []
        for text_id in group.text_object_ids:
            text_object = TextObject.objects(id=text_id).first()
            text_object_model = TextObjectModel(
                id=str(text_object.id),
                content=text_object.content,
                processed_content=text_object.processed_content,
                project_id=str(text_object.project_id),
                analog_count=0,
                user_rating_count=0
            )
            text_object_models.append(text_object_model)

        group_model = GroupModel(
            id=str(group.id),
            project_id=str(group.project_id),
            center_text=center_text_object_model,
            texts=text_object_models
        )
        group_models.append(group_model)

    print(f"Loaded {len(group_models)} group models.")

    text_object_models: list[TextObjectModel] = []
    for text_object in text_objects:
        text_object_model = TextObjectModel(
            id=str(text_object.id),
            content=text_object.content,
            processed_content=text_object.processed_content,
            project_id=str(text_object.project_id),
            analog_count=0,
            user_rating_count=0
        )
        text_object_models.append(text_object_model)

    similarity_threshold = 0.5

    print(f"Loaded {len(text_object_models)} text object models.")

    start = time.time()
    processed_text_objects = preprocess(text_object_models, 'russian')
    group_text_objects(processed_text_objects, group_models, similarity_threshold)

    print(f"Grouped {len(processed_text_objects)} text objects.")
    print('Time:', time.time() - start)


if __name__ == '__main__':
    main()
