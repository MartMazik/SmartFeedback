import re
import time
from collections import defaultdict

from langdetect import detect
from nltk.corpus import stopwords
from nltk.stem import SnowballStemmer
from nltk.tokenize import word_tokenize
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity


def preprocess_one_text(text, language):
    text = text.lower()

    # Проверка на пустой ввод
    if not text.strip():
        return []

    # Определение языка текста, если указано "none" или не указано значение
    if language.lower() == 'none':
        try:
            language = detect(text)
        except:
            language = 'english'  # По умолчанию, если определить язык не удалось

    # Удаление html-тегов
    text = re.sub(r'<[^>]+>', ' ', text)

    # Удаление ссылок
    text = re.sub(r'http\S+', ' ', text)

    # Удаление знаков препинания и других символов, кроме букв и пробелов
    text = re.sub(r'[^\w\s]', ' ', text)

    # Удаление цифр
    text = re.sub(r'\d+', ' ', text)

    # Удаление лишних пробелов
    text = re.sub(r'\s+', ' ', text)

    # Токенизация
    tokens = word_tokenize(text)

    # Удаление стоп-слов и стемминг
    stop_words = set(stopwords.words(language))
    tokens = [word for word in tokens if word not in stop_words]

    # Стемминг
    stemmer = SnowballStemmer(language)
    tokens = [stemmer.stem(word) for word in tokens]

    # Удаление слов длиной меньше 2 символов
    tokens = [word for word in tokens if len(word) >= 2]

    return tokens

def cluster_texts(texts, language='english', similarity_threshold=0.85):
    preprocessed_texts = [preprocess_one_text(text, language) for text in texts]

    # Step 1: Vectorize the preprocessed texts
    preprocessed_texts = [' '.join(tokens) for tokens in preprocessed_texts]
    vectorizer = TfidfVectorizer()
    tfidf_matrix = vectorizer.fit_transform(preprocessed_texts)

    # Step 2: Compute pairwise cosine similarities
    cosine_similarities = cosine_similarity(tfidf_matrix, tfidf_matrix)

    # Step 3: Cluster texts based on similarity
    clusters = defaultdict(list)
    num_texts = len(texts)

    for i in range(num_texts):
        for j in range(i + 1, num_texts):
            if cosine_similarities[i, j] >= similarity_threshold:
                clusters[i].append(j)
                clusters[j].append(i)

    # Step 4: Format clusters for output
    clustered_texts = []
    visited = set()

    for idx in range(num_texts):
        if idx not in visited:
            cluster = clusters[idx]
            cluster.append(idx)  # include the text itself in the cluster
            clustered_texts.append([texts[i] for i in cluster])
            visited.update(cluster)

    return clustered_texts

# Example usage:
if __name__ == "__main__":
    texts = [
    "Какой замечательный рыжий котенок! Его игра с мячиком под столом просто очаровательна.",
    "Серый котик так забавно пытается достать игрушки! Кажется, ему это очень интересно.",
    "Черный котенок такой наблюдательный, он явно стратег в этой компании.",
    "Момент, когда черный кот ловко прячет мячик под столом - просто гениально!",
    "Как мило они все вместе играют! Настоящие друзья.",
    "Веселая борьба за мячик-мышку - это просто шикарно! Не могу перестать улыбаться.",
    "Рыжий котенок такой ловкий, когда прячет игрушки! Обожаю его энергию.",
    "Серый кот так старательно пытается вытащить мячик, его усердие вдохновляет.",
    "Черный котик на стуле - это просто картина! Он такой милый наблюдатель.",
    "Как же мне нравится, когда они все вместе сидят на коврике и разглядывают друг друга.",
    "Момент, когда черный кот присоединяется к игре, такой трогательный!",
    "Эти котики такие милые и забавные, не могу оторвать глаз.",
    "Рыжий котик - настоящий шутник! Как он ловко всех запутывает.",
    "Серый котик так серьезно относится к игре, он такой сосредоточенный.",
    "Черный котик такой грациозный и умный, видно, что он главный стратег.",
    "Все вместе они создают настоящую гармонию и веселье на кухне.",
    "Как здорово, что они все такие разные, но такие дружные!",
    "Мячик-мышка - просто хит, котики не могут остановиться.",
    "Рыжий котенок, ловко прячущий игрушки, - настоящий акробат.",
    "Серый кот так забавно вытягивает лапку, пытаясь достать мячик.",
    "Черный котик, наблюдающий за игрой, добавляет атмосферу таинственности.",
    "В конце, когда они все вместе сидят на коврике, - это просто идиллия.",
    "Рыжий котенок такой активный и веселый, невозможно не улыбнуться.",
    "Серый кот такой внимательный и настойчивый, ему все под силу.",
    "Черный котик - просто воплощение грации и ума.",
    "Их игра с мячиком-мышкой - настоящий спектакль.",
    "Как же они все мило и дружно играют вместе!",
    "Момент, когда черный кот прячет мячик, показывает, какой он умный и ловкий.",
    "Котики на кухне создают такую уютную атмосферу.",
    "Очень трогательно, когда они все вместе сидят и разглядывают друг друга, радуясь своей дружбе.",
    ]

    # Measure execution time
    start_time = time.time()

    clusters = cluster_texts(texts, language='russian', similarity_threshold=0.3)

    end_time = time.time()
    execution_time = end_time - start_time
    print(f"Clustering took {execution_time} seconds")

    print("Clustered texts:")
    for cluster in clusters:
        print(cluster)
        print()
