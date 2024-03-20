import time

from mongoengine import connect

from _dev.config import MONGO_HOST, MONGO_DB, SIMILARITY_THRESHOLD
from _dev.mongoEntities.connect_text_object import ConnectTextObject
from _dev.mongoEntities.group_text_objects import GroupTextObject
from _dev.mongoEntities.text_object import TextObject
from services.comparison_service import compare_two_texts
from services.grouping_service import group_text
from services.preprocess_service import preprocess_one_text


def init_db():
    connect(db=MONGO_DB, host=MONGO_HOST)


def similar_text_objects(text_objects):
    total_pairs = len(text_objects) * (len(text_objects) - 1) // 2
    pair_count = 0

    for i in range(len(text_objects)):
        for j in range(i + 1, len(text_objects)):
            pair_count += 1
            if pair_count % (total_pairs // 100) == 0:
                print(f"Processed pair {pair_count} of {total_pairs}")

            first_text_object = text_objects[i]
            second_text_object = text_objects[j]
            similarity = compare_two_texts(first_text_object, second_text_object)

            if similarity < SIMILARITY_THRESHOLD:
                continue

            new_connect_text_object = ConnectTextObject(
                first_text_object_id=str(first_text_object.id),
                second_text_object_id=str(second_text_object.id),
                similarity=similarity
            )
            new_connect_text_object.save()


def preprocess_text_objects(text_objects):
    for text_object in text_objects:
        text_object.processed_content = preprocess_one_text(text_object.content, "russian")
        text_object.save()


def group_text_objects(text_objects):
    start_time = time.time()
    pair_count = 0
    # Получаем все группы из базы данных по проекту
    text_groups = GroupTextObject.objects(project_id=text_objects[0].project_id).all()
    # получаем все сравнения текстов из базы данных по проекту
    compare_text_objects = ConnectTextObject.objects(project_id=text_objects[0].project_id).all()
    compare_text_objects = list(compare_text_objects)
    new_compare_text_objects = []
    new_text_groups = []
    for text_object in text_objects:
        new_compare_texts, new_text_group = group_text(text_object, compare_text_objects, text_groups)
        if new_compare_texts is not None:
            for compare_text in new_compare_texts:
                new_compare_text_objects.append(compare_text)
        if new_text_group is not None:
            new_text_groups.append(new_text_group)
        pair_count += 1
        if pair_count % 100 == 0:
            print(f"Processed {pair_count} of {len(text_objects)} | Time: {time.time() - start_time} sec")
    ConnectTextObject.objects.insert_many(new_compare_text_objects)
    GroupTextObject.objects.insert_many(new_text_groups)


def main():
    init_db()

    # Get all text objects for a project
    project_id = '65f8dec3b2371d94c7fbbef9'
    text_objects = TextObject.objects(project_id=project_id).all()

    # Preprocess all text objects
    print("START. Preprocessing text objects...")
    # preprocess_text_objects(text_objects)
    print("END. Preprocessing completed.")

    # Compare all text objects
    print("START. Comparing text objects...")
    # similar_text_objects(text_objects)
    print("END. Comparison completed.")

    # Grouping text objects
    print("START. Grouping text objects...")
    group_text_objects(text_objects)

    # # Вывести группы с двумя и более текстами
    # # Группа: center_text_object
    # # Text: content
    # print("Groups:")
    # for group in GroupTextObject.objects(project_id=project_id).all():
    #     if len(group.text_object_ids) > 1:
    #         center_text = TextObject.objects(id=group.center_text_object_id).first()
    #         print(f"Group: {center_text.content}")
    #         for text_id in group.text_object_ids:
    #             text = TextObject.objects(id=text_id).first()
    #             print(f"Text: {text.content}")
    #         print("")

    print("END. Grouping completed.")


if __name__ == '__main__':
    main()
