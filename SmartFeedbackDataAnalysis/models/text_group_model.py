from pydantic import BaseModel

from models.text_object_model import TextObjectModel


class TextGroupModel(BaseModel):
    id: str
    project_id: str
    analog_count: int
    core_text: str
    center_text: TextObjectModel
    analog_texts: list[TextObjectModel]
