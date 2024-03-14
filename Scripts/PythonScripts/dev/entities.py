class Texts:
    def __init__(self, data):
        self.id = data.get('_id')
        self.is_deleted = data.get('IsDeleted')
        self.content = data.get('Content')
        self.processed_content = data.get('ProcessedContend')
        self.project_id = data.get('ProjectId')
        self.analog_count = data.get('AnalogCount')
        self.user_rating_count = data.get('UserRatingCount')
        self.rating_sum = data.get('RatingSum')

    def __str__(self):
        return f'{self.id} | {self.content}'


class Projects:
    def __init__(self, data):
        self.id = data.get('_id')
        self.name = data.get('Name')
        self.is_deleted = data.get('IsDeleted')
        self.language = data.get('Language')

    def __str__(self):
        return f'{self.id} | {self.name}'