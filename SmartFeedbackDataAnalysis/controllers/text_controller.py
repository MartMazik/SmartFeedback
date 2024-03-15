from typing import List

from fastapi import APIRouter

from models.text_model import TextModel

router = APIRouter()

@router.post("/preprocessing/one")
async def preprocessing_one(text_model: TextModel):
    return {"message": "Preprocessing one"}

@router.post("/preprocessing/few")
async def preprocessing_few(text_models: List[TextModel]):
    return {"message": "Preprocessing few"}

@router.post("/compare"