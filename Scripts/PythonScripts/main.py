import pandas as pd

# Получить даненые из монго, обработать их и вывести в консоль
from dev.mongodb import MongoDB
from preprocessing_module import preprocess_text
from tfidf import calculate_tfidf


def start_preprocessing():
    try:
        # Подключаемся к MongoDB и получаем тексты
        mongo = MongoDB('mongodb://localhost:27017/', 'smart_feedback')
        texts = mongo.get_all('texts')

        proceed_texts = []

        # Предварительная обработка текстов
        for text in texts:
            proceed_texts.append(preprocess_text(text['Content']))
            print(text)

        # Рассчитываем TF-IDF
        end_data = calculate_tfidf(proceed_texts)

        # Преобразуем результат в DataFrame и сохраняем в CSV файл
        df = pd.DataFrame(end_data.toarray())  # Преобразуем разреженную матрицу в DataFrame
        df.to_csv('data.csv', index=False)

        print("Данные успешно сохранены в файл 'data.csv'")

    except Exception as e:
        print("Произошла ошибка:", e)


if __name__ == '__main__':
    start_preprocessing()
