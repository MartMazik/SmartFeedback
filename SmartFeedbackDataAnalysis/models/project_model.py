from pydantic import BaseModel


class ProjectModel(BaseModel):
    id: str
    user_id: str
    title: str
    language: str
    similarity_threshold: float
