from fastapi import APIRouter

from models.project_model import ProjectModel
from models.text_group_model import TextGroupModel
from models.text_object_model import TextObjectModel
from services.grouping_service import group_text_objects
from services.preprocess_service import preprocess
from services.searching_service import search_projects_method, search_texts_method

router = APIRouter()


@router.get("/")
async def read_root():
    return {"message": "Welcome to the Text Comparison API"}


@router.post("/preprocessing/one")
async def preprocessing_one(text_object_model: TextObjectModel, project_model: ProjectModel):
    return preprocess([text_object_model], project_model.language)[0]


@router.post("/preprocessing/few")
async def preprocessing_few(text_object_models: list[TextObjectModel], project_model: ProjectModel):
    return preprocess(text_object_models, project_model.language)


@router.post("/comparison")
async def comparison(group_models: list[TextGroupModel], text_object_models: list[TextObjectModel],
                     project_model: ProjectModel):
    return group_text_objects(text_object_models, group_models, project_model.similarity_threshold)


@router.post("/preprocess-comparison")
async def preprocessing_comparison(group_models: list[TextGroupModel], text_object_models: list[TextObjectModel],
                                   project_model: ProjectModel):
    processed_text_objects = preprocess(text_object_models, project_model.language)
    return group_text_objects(processed_text_objects, group_models, project_model.similarity_threshold)


@router.post("/search-in-text")
async def search_text(text_group_models: list[TextGroupModel], project_model: ProjectModel, search_line: str):
    return search_texts_method(text_group_models, search_line, project_model)


@router.post("/search-in-project")
async def search_project(project_models: list[ProjectModel], search_line: str):
    return search_projects_method(project_models, search_line)
