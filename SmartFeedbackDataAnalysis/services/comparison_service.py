def compare_two_texts(first_text_object, second_text_object):
    return jaccard_similarity(first_text_object.processed_content, second_text_object.processed_content)


def jaccard_similarity(text1, text2):
    words_text1 = set(text1)
    words_text2 = set(text2)

    intersection = len(words_text1.intersection(words_text2))

    union = len(words_text1) + len(words_text2) - intersection

    similarity = intersection / union if union != 0 else 0

    return similarity
