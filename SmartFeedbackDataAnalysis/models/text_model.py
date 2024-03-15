from pydantic import BaseModel


class TextModel(BaseModel):
    id: int
    content: str
    processed_content: str