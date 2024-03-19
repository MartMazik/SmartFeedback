import re
import nltk
from nltk import SnowballStemmer
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize

nltk.download('punkt')
nltk.download('stopwords')

def preprocess_few_texts(texts, language='english'):
    preprocessed_texts = []

    for t in texts:
        preprocessed_text = preprocess_one_text(t, language)
        preprocessed_texts.append(preprocessed_text)

    return preprocessed_texts


def preprocess_one_text(text, language='english'):
    text = text.lower()

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

    tokens = word_tokenize(text)

    stop_words = set(stopwords.words(language))
    tokens = [word for word in tokens if word not in stop_words]

    stemmer = SnowballStemmer(language)
    tokens = [stemmer.stem(word) for word in tokens]

    tokens = [word for word in tokens if len(word) > 2]

    return tokens
