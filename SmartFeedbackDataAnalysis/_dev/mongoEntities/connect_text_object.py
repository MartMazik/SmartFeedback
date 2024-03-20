from mongoengine import Document, BooleanField, FloatField, StringField


class ConnectTextObject(Document):
    is_deleted = BooleanField(default=False)
    first_text_object_id = StringField()
    second_text_object_id = StringField()
    project_id = StringField()
    similarity = FloatField()
