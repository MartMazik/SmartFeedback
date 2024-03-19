from sklearn.feature_extraction.text import TfidfVectorizer


def compare_texts(texts):
    compare_texts = []
    for i in range(len(texts)):
        compare_texts.append(texts[i].processed_content)

    tfidf_vectorizer = TfidfVectorizer()
    tfidf_matrix = tfidf_vectorizer.fit_transform(compare_texts)

    return "tfidf_matrix"


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
