from sklearn.feature_extraction.text import TfidfVectorizer

from _dev.mongoEntities.connect_text_object import ConnectTextObject


def compare_texts(texts):
    compare_texts = []
    for i in range(len(texts)):
        compare_texts.append(texts[i].processed_content)

    tfidf_vectorizer = TfidfVectorizer()
    tfidf_matrix = tfidf_vectorizer.fit_transform(compare_texts)

    return "tfidf_matrix"


def compare_two_texts_with_db_save(text_1, text_2, connect_texts_list):
    # Ищем в списке уже сравнивавшихся текстов
    for connect_text in connect_texts_list:
        if (connect_text.first_text_object_id == text_1.id and connect_text.second_text_object_id == text_2.id) or \
                (connect_text.first_text_object_id == text_2.id and connect_text.second_text_object_id == text_1.id):
            return connect_text.similarity

    similarity_result = compare_two_texts(text_1, text_2)
    new_connect_text_object = ConnectTextObject(
        first_text_object_id=str(text_1.id),
        second_text_object_id=str(text_2.id),
        similarity=similarity_result
    )
    connect_texts_list.append(new_connect_text_object)
    return new_connect_text_object


def compare_two_texts(first_text_object, second_text_object):
    return jaccard_similarity(first_text_object.processed_content, second_text_object.processed_content)


def jaccard_similarity(text1, text2):
    # Преобразовать списки в множества
    words_text1 = set(text1)
    words_text2 = set(text2)

    # Найти количество общих слов
    intersection = len(words_text1.intersection(words_text2))

    # Найти общее количество уникальных слов
    union = len(words_text1) + len(words_text2) - intersection

    # Рассчитать меру Жаккара
    jaccard_similarity = intersection / union if union != 0 else 0

    return jaccard_similarity
