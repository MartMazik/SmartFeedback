import re
import nltk
from nltk.corpus import stopwords
from nltk.tokenize import word_tokenize
from nltk.stem import WordNetLemmatizer

nltk.download('punkt')
nltk.download('stopwords')
nltk.download('wordnet')


def preprocess_text(text, language='english'):
    text = text.lower()

    text = re.sub(r'[^\w\s]', ' ', text)

    text = re.sub(r'\d+', ' ', text)

    text = re.sub(r'\s+', ' ', text)

    tokens = word_tokenize(text)

    stop_words = set(stopwords.words(language))
    tokens = [word for word in tokens if word not in stop_words]

    lemmatizer = WordNetLemmatizer()
    tokens = [lemmatizer.lemmatize(word) for word in tokens]

    preprocessed_text = ' '.join(tokens)

    return preprocessed_text
