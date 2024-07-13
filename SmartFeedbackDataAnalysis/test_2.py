import re

import matplotlib.pyplot as plt
import pandas as pd
from langdetect import detect
from nltk.corpus import stopwords
from nltk.stem import SnowballStemmer
from nltk.tokenize import word_tokenize
from sklearn.cluster import MeanShift
from sklearn.decomposition import PCA
from sklearn.feature_extraction.text import TfidfVectorizer

# Предложения для сравнения
texts = [
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
preprocessed_sentences = [preprocess_one_text(sentence, 'russian') for sentence in texts]

# Объединяем предобработанные тексты в строки
preprocessed_sentences_joined = [' '.join(tokens) for tokens in preprocessed_sentences]

# Создаем TF-IDF векторизатор
tfidf_vectorizer = TfidfVectorizer()
X = tfidf_vectorizer.fit_transform(preprocessed_sentences_joined)


# Применим алгоритм Mean Shift Clustering
mean_shift = MeanShift()
labels = mean_shift.fit_predict(X.toarray())

# Выведем результаты кластеризации
df = pd.DataFrame({'Text': texts, 'Cluster': labels})

# Группируем тексты по кластерам и выводим их
clusters = df.groupby('Cluster')['Text'].apply(list)
for cluster_id, texts in clusters.items():
    print(f"Кластер {cluster_id}:")
    for text in texts:
        print(f"* {text}")
    print()

# Визуализируем результаты с помощью PCA
pca = PCA(n_components=2)
X_pca = pca.fit_transform(X.toarray())

plt.figure(figsize=(10, 7))
plt.scatter(X_pca[:, 0], X_pca[:, 1], c=labels, cmap='viridis', marker='o')
for i, text in enumerate(texts):
    plt.annotate(text, (X_pca[i, 0], X_pca[i, 1]), fontsize=8)
plt.title("Mean Shift Clustering of Text Data (Russian)")
plt.xlabel("PCA Component 1")
plt.ylabel("PCA Component 2")
plt.show()