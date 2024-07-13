from pydantic import BaseModel


class TextObjectModel(BaseModel):
    id: str
    content: str
    processed_content: list[str]
    project_id: str
    user_rating_count: int