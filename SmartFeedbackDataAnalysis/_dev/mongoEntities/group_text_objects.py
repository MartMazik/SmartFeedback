from mongoengine import Document, ListField, BooleanField, StringField


class GroupTextObject(Document):
    is_deleted = BooleanField(default=False)
    project_id = StringField()
    center_text_object_id = StringField()
    text_object_ids = ListField(StringField())
