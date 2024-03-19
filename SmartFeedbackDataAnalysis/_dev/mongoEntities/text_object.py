from mongoengine import Document, StringField, ListField, BooleanField, IntField


class TextObject(Document):
    is_deleted = BooleanField(default=False)
    content = StringField()
    processed_content = ListField(StringField())
    project_id = StringField()
    analog_count = IntField()
    user_rating_count = IntField()
    rating_sum = IntField()
