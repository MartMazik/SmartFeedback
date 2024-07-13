import re
import nltk
from nltk import SnowballStemmer
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from langdetect import detect

from models.text_object_model import TextObjectModel

nltk.download('punkt')
nltk.download('stopwords')


def preprocess(texts: list[TextObjectModel], language):
    for t in texts:
        t.processed_content = preprocess_one_text(t.content, language)
    return texts


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
