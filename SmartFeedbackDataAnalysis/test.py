import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from nltk.corpus import stopwords
from nltk.stem import SnowballStemmer
from nltk.tokenize import word_tokenize
import re
import nltk
from langdetect import detect

# Предложения для сравнения
sentences = [
    "Солнце светит ярко на небе.",
    "Луна светит ночью на небе.",
    "Кошка лежит на окне и мурлычет.",
    "Собака бегает по двору и лает на прохожих.",
    "Птицы поют веселые песни на ветках деревьев.",
    "Рыбы плавают в прозрачной речной воде.",
    "Медведь спит под деревом в тени.",
    "Тигр охотится на добычу в дремучем лесу.",
    "Автомобиль мчится по скоростной трассе.",
    "Велосипедист катается по узкой горной дороге.",
    "Девочка читает книгу на скамейке в парке.",
    "Мальчик играет в футбол на стадионе с друзьями.",
    "Цветы расцветают весной в местном саду.",
    "Деревья колышутся от ветра в зеленом лесу.",
    "Дом стоит на углу улицы, окруженный газоном.",
    "Здание высотой 20 этажей торчит над всеми соседними домами.",
    "Красная роза растет в саду у стены.",
    "Сирень расцветает весной, наполняя воздух своим ароматом.",
    "Водопад возвышается над скалами, создавая впечатляющий вид.",
    "Ручей мягко шепчет среди камней в горах."
]


# Предобработка текстов
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


# Предобработка текстов
preprocessed_sentences = [preprocess_one_text(sentence, 'russian') for sentence in sentences]

# Объединяем предобработанные тексты в строки
preprocessed_sentences_joined = [' '.join(tokens) for tokens in preprocessed_sentences]

# Создаем TF-IDF векторизатор
tfidf_vectorizer = TfidfVectorizer()
tfidf_matrix = tfidf_vectorizer.fit_transform(preprocessed_sentences_joined)

# Вычисляем матрицу сходства (косинусное расстояние между TF-IDF векторами)
similarity_matrix = cosine_similarity(tfidf_matrix)

# Задаем порог сходства, выше которого будем считать предложения схожими
threshold = 0.001

# Список для отслеживания уже выведенных предложений
printed_indices = []

# Выводим предложения по группам на основе сходства
for i in range(len(sentences)):
    if i in printed_indices:
        continue

    similar_indices = [j for j in range(len(sentences)) if
                       similarity_matrix[i, j] >= threshold and j not in printed_indices]
    if len(similar_indices) > 1:
        similar_sentences = [sentences[j] for j in similar_indices]
        print(f"Предложение {i + 1} ({sentences[i]}) есть сходство на:")
        for idx in similar_indices:
            printed_indices.append(idx)
            print(f"    - Предложение {idx + 1} ({sentences[idx]})")
        print()

# Для предложений, у которых нет сходства выше порога
unprinted_indices = [idx for idx in range(len(sentences)) if idx not in printed_indices]
for idx in unprinted_indices:
    print(f"Предложение {idx + 1} ({sentences[idx]}) не имеет достаточного сходства с другими предложениями.")
