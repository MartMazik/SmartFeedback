from pydantic import BaseModel


class ProjectModel(BaseModel):
    id: int
    name: str
    is_deleted: bool
    language: str