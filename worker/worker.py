import psutil
import platform


def cpu_usage():
    return {
        "cpu_percent": psutil.cpu_percent(interval=1)
    }


def memory_usage():
    mem = psutil.virtual_memory()
    return {
        "used_mb": round(mem.used / (1024 * 1024)),
        "total_mb": round(mem.total / (1024 * 1024)),
        "percent": mem.percent
    }


def disk_usage():
    path = "C:\\" if platform.system() == "Windows" else "/"
    disk = psutil.disk_usage(path)

    return {
        "used_gb": round(disk.used / (1024 * 1024 * 1024)),
        "total_gb": round(disk.total / (1024 * 1024 * 1024)),
        "percent": disk.percent
    }


def collect_all_metrics():
    cpu = cpu_usage()
    memory = memory_usage()
    disk = disk_usage()

    return {
        "host": platform.node(),
        "cpuPercent": cpu["cpu_percent"],
        "memUsedMb": memory["used_mb"],
        "memTotalMb": memory["total_mb"],
        "memPercent": memory["percent"],
        "diskUsedGb": disk["used_gb"],
        "diskTotalGb": disk["total_gb"],
        "diskPercent": disk["percent"]
    }


TASK_HANDLERS = {
    "cpu_usage": cpu_usage,
    "memory_usage": memory_usage,
    "disk_usage": disk_usage
}


def process_tasks(tasks):
    results = []

    for task in tasks:
        task_id = task.get("id")
        task_type = task.get("type")

        if task_type not in TASK_HANDLERS:
            results.append({
                "id": task_id,
                "type": task_type,
                "status": "error",
                "error": "Unsupported task type"
            })
            continue

        try:
            data = TASK_HANDLERS[task_type]()
            results.append({
                "id": task_id,
                "type": task_type,
                "status": "ok",
                "data": data
            })
        except Exception as e:
            results.append({
                "id": task_id,
                "type": task_type,
                "status": "error",
                "error": str(e)
            })

    return {"results": results}