class ProjectModel:
    def __init__(self, data):
        self.id = data.get('_id')
        self.name = data.get('Name')
        self.is_deleted = data.get('IsDeleted')
        self.language = data.get('Language')

    def __str__(self):
        return f'{self.id} | {self.name}'