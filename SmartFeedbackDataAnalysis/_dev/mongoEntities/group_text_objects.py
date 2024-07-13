from mongoengine import Document, ListField, BooleanField, StringField, ObjectIdField


class GroupTextObject(Document):
    is_deleted = BooleanField(default=False)
    project_id = ObjectIdField()
    center_text_object_id = ObjectIdField()
    text_object_ids = ListField(ObjectIdField())
