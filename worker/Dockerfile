FROM python:3.11-slim

WORKDIR /app

COPY . .

RUN pip3 install --no-cache-dir psutil --break-system-packages

CMD ["python", "worker.py"]