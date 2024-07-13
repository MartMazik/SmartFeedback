from models.text_group_model import TextGroupModel
from models.project_model import ProjectModel
from services.comparison_service import compare_two_texts, jaccard_similarity
from services.preprocess_service import preprocess_one_text


def search_projects_method(projects: list[ProjectModel], search_line: str):
    preprocess_search_line = preprocess_one_text(search_line)
    temp_projects = []

    for project in projects:
        similarity = jaccard_similarity(preprocess_search_line, preprocess_one_text(project.title))
        if similarity > 0.05:
            temp_projects.append((project, similarity))

    temp_projects.sort(key=lambda x: x[1], reverse=True)
    sorted_projects = [project for project, similarity in temp_projects]

    return sorted_projects


def search_texts_method(text_group_models: list[TextGroupModel], search_line: str, project_model: ProjectModel):
    preprocess_search_line = preprocess_one_text(search_line, project_model.language)
    temp_text_groups = []

    for text_group_model in text_group_models:
        similarity = jaccard_similarity(preprocess_search_line, text_group_model.center_text.processed_content)
        if similarity > 0.05:
            temp_text_groups.append((text_group_model, similarity))

    temp_text_groups.sort(key=lambda x: x[1], reverse=True)
    sorted_text_groups = [text_group_model for text_group_model, similarity in temp_text_groups]

    return sorted_text_groups
