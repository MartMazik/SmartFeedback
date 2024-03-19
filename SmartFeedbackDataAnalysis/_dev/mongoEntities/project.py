from mongoengine import Document, StringField, BooleanField


class Project(Document):
    is_deleted = BooleanField(default=False)
    title = StringField()
    language = StringField()