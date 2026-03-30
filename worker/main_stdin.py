import json
import sys
from worker_stdin import process_tasks


def main():
    try:
        payload = json.load(sys.stdin)

        if "tasks" not in payload or not isinstance(payload["tasks"], list):
            raise ValueError("Invalid payload")

        tasks = payload.get("tasks", [])
        result = process_tasks(tasks)

        print(json.dumps(result, indent=2))

    except Exception as e:
        print(json.dumps({
            "error": str(e)
        }, indent=2))


if __name__ == "__main__":
    main()