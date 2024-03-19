from fastapi import FastAPI

from _dev.mongoDB import MongoDB
from controllers import text_controller

app = FastAPI()

app.include_router(text_controller.router)


def start():
    mongo = MongoDB("mongodb://localhost:27017", "smart_feedback")
    texts = mongo.get_all("texts")

    text_models = []
    for text in texts:
        text_models.append({
            "id": text["_id"],
            "content": text["Content"],
            "processed_content": text["ProcessedContent"]
        })
        print(text["_id"] + " | " + text["Content"])


if __name__ == '__main__':
    start()
