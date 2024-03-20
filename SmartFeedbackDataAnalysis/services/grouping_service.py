from _dev.config import SIMILARITY_THRESHOLD
from _dev.mongoEntities.connect_text_object import ConnectTextObject
from _dev.mongoEntities.group_text_objects import GroupTextObject
from _dev.mongoEntities.text_object import TextObject
from services.comparison_service import compare_two_texts_with_db_save


def group_text(text, compare_text_objects, text_groups):
    if not text_groups:
        new_group = GroupTextObject(
            project_id=text.project_id,
            center_text_object_id=str(text.id),
            text_object_ids=[str(text.id)]
        )
        return None, new_group

    # Иначе ищем группу, в которую можно добавить текст
    is_grouped = False
    new_compare_text_objects = []
    for group in text_groups:
        center_text = TextObject.objects(id=group.center_text_object_id).first()
        compare_text = compare_two_texts_with_db_save(text, center_text, compare_text_objects)
        compare_text_objects.append(compare_text)
        new_compare_text_objects.append(compare_text)
        similarity = compare_text.similarity
        if similarity >= SIMILARITY_THRESHOLD:
            group.text_object_ids.append(str(text.id))
            group.save()
            is_grouped = True
            update_group_center(group, compare_text_objects)

    if not is_grouped:
        new_group = GroupTextObject(
            project_id=text.project_id,
            center_text_object_id=str(text.id),
            text_object_ids=[str(text.id)]
        )

    return new_compare_text_objects, new_group


def update_group_center(group, compare_text_objects):
    # получаем все тексты из группы
    text_objects = TextObject.objects(id__in=group.text_object_ids).all()
    # TODO считаем среднее значение для каждого слова, выбираем текст, который ближе всего к среднему значению

    # Сравниваем все тексты в группе между собой и выбираем тот, который получил наибольшую схожесть со всеми остальными в сумме
    max_similarity = 0
    center_text = None
    for text in text_objects:
        similarity = 0
        for text2 in text_objects:
            similarity += compare_two_texts_with_db_save(text, text2, compare_text_objects).similarity
        if similarity > max_similarity:
            max_similarity = similarity
            center_text = text
    group.center_text_object_id = str(center_text.id)
    group.save()
