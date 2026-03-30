from fastapi import FastAPI
from worker import collect_all_metrics, process_tasks

app = FastAPI(title="Proceso Worker API")


@app.get("/health")
def health():
    return {"status": "ok"}


@app.get("/metrics")
def get_metrics():
    return collect_all_metrics()


@app.post("/tasks/run")
def run_tasks(payload: dict):
    tasks = payload.get("tasks")

    if not isinstance(tasks, list):
        return {
            "error": "Invalid payload. 'tasks' must be a list."
        }

    return process_tasks(tasks)