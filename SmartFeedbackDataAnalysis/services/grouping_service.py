from models.text_group_model import TextGroupModel
from models.text_object_model import TextObjectModel
from services.comparison_service import compare_two_texts


# TODO считаем среднее значение для каждого слова, выбираем текст, который ближе всего к среднему значению
def update_group_center(group_model: TextGroupModel):
    max_similarity = 0
    for text in group_model.analog_texts:
        similarity = 0
        for text2 in group_model.analog_texts:
            similarity += compare_two_texts(text, text2)
        if similarity > max_similarity:
            max_similarity = similarity
            group_model.center_text = text


def group_text(text: TextObjectModel, group_models: list[TextGroupModel], similarity_threshold: float):
    if not group_models:
        return TextGroupModel(
            id='',
            project_id=text.project_id,
            analog_count=1,
            core_text=text.content,
            center_text=text,
            analog_texts=[text]
        )

    is_grouped = False
    for group_model in group_models:
        similarity = compare_two_texts(text, group_model.center_text)

        if similarity >= similarity_threshold:
            group_model.analog_texts.append(text)
            is_grouped = True
            update_group_center(group_model)

    if not is_grouped:
        return TextGroupModel(
            id='',
            project_id=text.project_id,
            analog_count=1,
            core_text=text.content,
            center_text=text,
            analog_texts=[text]
        )

    return None


def group_text_objects(texts: list[TextObjectModel], groups: list[TextGroupModel], similarity_threshold: float):
    for text in texts:
        new_group = group_text(text, groups, similarity_threshold)
        if new_group is not None:
            groups.append(new_group)

    for group in groups:
        group.analog_count = len(group.analog_texts)

    # После того, как все тексты были сгруппированы, сравниваем центральные тексты групп между собой и если они
    # схожи, объединяем группы в одну из всех аналогов делаем так, чтобы в группе не было одинаковых текстов и
    # выбираем центральный текст

    for group in groups:
        for group2 in groups:
            if group != group2:
                similarity = compare_two_texts(group.center_text, group2.center_text)
                if similarity >= similarity_threshold:
                    group.analog_texts += group2.analog_texts
                    group.analog_count = len(group.analog_texts)
                    groups.remove(group2)
                    update_group_center(group)

    return groups
