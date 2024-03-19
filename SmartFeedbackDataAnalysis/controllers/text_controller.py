from typing import List

from fastapi import APIRouter

from models.text_model import TextModel
from services.preprocess_service import preprocess_one_text
from services.comparison_service import compare_texts

router = APIRouter()


@router.get("/")
async def read_root():
    return {"message": "Welcome to the Text Comparison API"}


@router.post("/preprocessing/one")
async def preprocessing_one(text_model: TextModel):
    text_model.processed_content = preprocess_one_text(text_model.content)

    return text_model


@router.post("/preprocessing/few")
async def preprocessing_few(text_models: List[TextModel]):
    for text_model in text_models:
        text_model.processed_content = preprocess_one_text(text_model.content)

    return text_models


@router.post("/comparison")
async def comparison(text_models: List[TextModel]):
    return compare_texts(text_models)


@router.post("/preprocessing-comparison")
async def preprocessing_comparison(text_models: List[TextModel]):
    for text_model in text_models:
        text_model.processed_content = preprocess_one_text(text_model.content)

    return compare_texts(text_models)
