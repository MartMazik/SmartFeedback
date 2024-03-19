import pymongo


class MongoDB:
    def __init__(self, connection_string, db_name):
        self.client = pymongo.MongoClient(connection_string)
        self.db = self.client[db_name]

    def get_collection(self, collection_name):
        return self.db[collection_name]

    def get_all(self, collection_name):
        collection = self.get_collection(collection_name)
        return list(collection.find())

    def get_by_id(self, collection_name, id):
        collection = self.get_collection(collection_name)
        return collection.find_one({"_id": id})

    def create(self, collection_name, data):
        collection = self.get_collection(collection_name)
        result = collection.insert_one(data)
        return result.inserted_id

    def update(self, collection_name, id, data):
        collection = self.get_collection(collection_name)
        result = collection.update_one({"_id": id}, {"$set": data})
        return result.modified_count > 0
