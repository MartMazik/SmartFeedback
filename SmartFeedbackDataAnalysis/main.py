from fastapi import FastAPI
from controllers import text_controller

app = FastAPI(
    title="Text Comparison API",
    description="API для сравнения текстов и проектов с автоматической генерацией документации",
    version="1.0.0",
    docs_url="/docs",
    redoc_url="/redoc"
)

app.include_router(text_controller.router)

if __name__ == '__main__':
    import uvicorn

    uvicorn.run(app, host="127.0.0.1", port=8010, reload=False)
