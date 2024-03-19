from mongoengine import connect, ObjectIdField

from _dev.mongoEntities.connect_text_object import ConnectTextObject
from _dev.mongoEntities.text_object import TextObject
from services.comparison_service import compare_two_texts
from services.preprocess_service import preprocess_one_text

constants = {
    'MONGO_DB': 'smart_feedback',
    'MONGO_HOST': 'mongodb://localhost:27017',
    'SIMILARITY_THRESHOLD': 0.25
}


def init_db():
    connect(db=constants['MONGO_DB'], host=constants['MONGO_HOST'])


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

            if similarity < constants['SIMILARITY_THRESHOLD']:
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


def main():
    init_db()

    # Get all text objects for a project
    project_id = '65f8dec3b2371d94c7fbbef9'
    text_objects = TextObject.objects(project_id=project_id).all()

    # Preprocess all text objects
    print("START. Preprocessing text objects...")
    preprocess_text_objects(text_objects)
    print("END. Preprocessing completed.")

    # Compare all text objects
    print("START. Comparing text objects...")
    #similar_text_objects(text_objects)
    print("END. Comparison completed.")


if __name__ == '__main__':
    main()
