import subprocess
import json
import sys
from pathlib import Path

MAIN = Path(__file__).parent / "main.py"

def run_main(input_json):
    process = subprocess.Popen(
        [sys.executable, MAIN],
        stdin=subprocess.PIPE,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )

    stdout, stderr = process.communicate(json.dumps(input_json))
    return stdout, stderr


def test_main_valid_input():
    payload = {
        "tasks": [
            {"id": "1", "type": "cpu_usage"}
        ]
    }

    out, err = run_main(payload)
    result = json.loads(out)

    assert "results" in result
    assert result["results"][0]["status"] == "ok"


def test_main_invalid_payload():
    out, err = run_main({"foo": "bar"})
    result = json.loads(out)

    assert "error" in result
