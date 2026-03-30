import worker

def test_cpu_usage():
    result = worker.cpu_usage()
    assert "cpu_percent" in result
    assert isinstance(result["cpu_percent"], (int, float))


def test_memory_usage():
    result = worker.memory_usage()
    assert "used_mb" in result
    assert "total_mb" in result
    assert result["used_mb"] > 0


def test_disk_usage():
    result = worker.disk_usage()
    assert "used_gb" in result
    assert "total_gb" in result


def test_process_tasks_ok():
    tasks = [
        {"id": "1", "type": "cpu_usage"},
        {"id": "2", "type": "memory_usage"}
    ]

    result = worker.process_tasks(tasks)

    assert "results" in result
    assert len(result["results"]) == 2
    assert result["results"][0]["status"] == "ok"


def test_process_tasks_invalid():
    tasks = [{"id": "1", "type": "unknown"}]
    result = worker.process_tasks(tasks)

    assert result["results"][0]["status"] == "error"
