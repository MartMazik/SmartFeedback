from fastapi import FastAPI
from controllers import text_controller

if __name__ == '__main__':
    app = FastAPI()

    app.include_router(text_controller.router)