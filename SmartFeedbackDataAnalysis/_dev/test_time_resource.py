import random
import time
from mongoengine import Q
from mongoengine import connect

from _dev.config import MONGO_DB, MONGO_HOST
from _dev.mongoEntities.connect_text_object import ConnectTextObject
from _dev.mongoEntities.text_object import TextObject
from services.comparison_service import compare_two_texts


def init_db():
    connect(db=MONGO_DB, host=MONGO_HOST)


def print_progress(counter, total_count, main_time):
    if counter == 100 or counter == 100_000 or counter % (total_count // 10) == 0:
        print(f"Processed {counter / total_count * 100}% | Time: {time.time() - main_time}")


def compare_and_save_individual(text_objects):
    print(f"START Processed 1. {len(text_objects)}")
    process_count = 0
    start_time = time.time()

    for i in range(len(text_objects)):
        first_text_object = text_objects[i]
        for j in range(i + 1, len(text_objects)):
            second_text_object = text_objects[j]
            similarity = compare_two_texts(first_text_object, second_text_object)
            new_connect_text_object = ConnectTextObject(
                first_text_object_id=str(first_text_object.id),
                second_text_object_id=str(second_text_object.id),
                similarity=similarity,
                project_id=first_text_object.project_id
            )
            new_connect_text_object.save()
            process_count += 1
            print_progress(process_count, (len(text_objects) ** 2 - len(text_objects)) / 2, start_time)

    print(f"END Processed 1. {len(text_objects) ** 2} | Time: {time.time() - start_time}")


def compare_and_save_batch(text_objects):
    print(f"START Processed 2. {len(text_objects)}")
    process_count = 0
    start_time = time.time()
    connect_text_objects = []

    for i in range(len(text_objects)):
        first_text_object = text_objects[i]
        for j in range(i + 1, len(text_objects)):
            second_text_object = text_objects[j]
            similarity = compare_two_texts(first_text_object, second_text_object)
            connect_text_objects.append(ConnectTextObject(
                first_text_object_id=str(first_text_object.id),
                second_text_object_id=str(second_text_object.id),
                similarity=similarity,
                project_id=first_text_object.project_id
            ))
            process_count += 1
            print_progress(process_count, (len(text_objects) ** 2 - len(text_objects)) / 2, start_time)

    # Сохранить в базу данных не более 100000 записей за раз
    for i in range(0, len(connect_text_objects), 100000):
        ConnectTextObject.objects.insert(connect_text_objects[i:i + 100000])
    print(f"END Processed 2. {len(text_objects) ** 2} | Time: {time.time() - start_time}")


# Два цикла сравнения текстов между собой - ищем в БД сохраненный результат сравнения
def compare_in_database_individual(text_objects):
    print(f"START Processed 3. {len(text_objects)}")
    process_count = 0
    start_time = time.time()

    similarity = 0
    for i in range(len(text_objects)):
        first_text_object = text_objects[i]
        for j in range(i + 1, len(text_objects)):
            second_text_object = text_objects[j]
            id1 = str(first_text_object.id)
            id2 = str(second_text_object.id)
            connect_text_object = ConnectTextObject.objects(
                Q(first_text_object_id=id1, second_text_object_id=id2) |
                Q(first_text_object_id=id2, second_text_object_id=id1)
            ).first()
            if connect_text_object is None:
                print("connect_text_object is None")
                continue
            similarity += connect_text_object.similarity
            process_count += 1
            print_progress(process_count, (len(text_objects) ** 2 - len(text_objects)) / 2, start_time)

    print(f"END Processed 3. {len(text_objects) ** 2} | Similarity: {similarity} |Time: {time.time() - start_time}")


# Два цикла сравнения текстов между собой - выкачиваем все результаты сравнения из БД и ищем нужное сравенение в памяти
def compare_in_database_batch(text_objects):
    print(f"START Processed 4. {len(text_objects)}")
    process_count = 0
    start_time = time.time()
    connect_text_objects = ConnectTextObject.objects(project_id=text_objects[0].project_id).all()
    connect_text_objects = list(connect_text_objects)
    similarity = 0

    for i in range(len(text_objects)):
        first_text_object = text_objects[i]
        for j in range(i + 1, len(text_objects)):
            second_text_object = text_objects[j]
            similarity_result = 0
            for connect_text in connect_text_objects:
                if (connect_text.first_text_object_id == str(
                        first_text_object.id) and connect_text.second_text_object_id == str(second_text_object.id)) or \
                        (connect_text.first_text_object_id == str(
                            second_text_object.id) and connect_text.second_text_object_id == str(first_text_object.id)):
                    similarity_result = connect_text.similarity
                    break
            similarity += similarity_result
            process_count += 1
            print_progress(process_count, (len(text_objects) ** 2 - len(text_objects)) / 2, start_time)

    print(f"END Processed 4. {len(text_objects) ** 2} | Similarity: {similarity} | Time: {time.time() - start_time}")


# Два цикла сравнения текстов между собой - Сравниваем тексты между собой с помощью функции
def compare_in_function(text_objects):
    print(f"START Processed 5. {len(text_objects)}")
    start_time = time.time()
    similarity = 0

    for i in range(len(text_objects)):
        first_text_object = text_objects[i]
        for j in range(i + 1, len(text_objects)):
            second_text_object = text_objects[j]
            similarity += compare_two_texts(first_text_object, second_text_object)

    print(f"END Processed 5. {len(text_objects) ** 2} | Similarity: {similarity} | Time: {time.time() - start_time}")


def main():
    init_db()

    text_objects = TextObject.objects()
    random_text_objects_2 = random.sample(list(text_objects), 2000)

    # print("\n\n Delete processed DB \n\n")
    # ConnectTextObject.objects().delete()
    # compare_and_save_individual(random_text_objects_1)

    # print("\n\n Delete processed DB \n\n")
    # ConnectTextObject.objects().delete()
    # compare_and_save_batch(text_objects)
    print("\n\n")
    compare_in_function(random_text_objects_2)
    print("\n\n")
    compare_in_database_individual(random_text_objects_2)
    print("\n\n")
    compare_in_database_batch(random_text_objects_2)
    print("\n\n")


if __name__ == "__main__":
    main()
