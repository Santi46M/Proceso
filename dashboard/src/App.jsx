import { useEffect, useState } from "react";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts";
import "./App.css";

//const API_BASE = "http://192.168.1.13:5180";
const API_BASE = `${window.location.protocol}//${window.location.hostname}:5180`;
export default function App() {
    const [data, setData] = useState(null);
    const [err, setErr] = useState("");
    const [loading, setLoading] = useState(true);
    const [lastUpdate, setLastUpdate] = useState("");
    const [history, setHistory] = useState([]);
    const chartData = [...history]
        .reverse()
        .map((m) => ({
            time: new Date(m.createdAt).toLocaleTimeString(),
            cpu: Number(m.cpuPercent),
            memory: Number(m.memPercent),
            disk: Number(m.diskPercent),
        }));

    async function load() {
        try {
            setErr("");
            setLoading(true);

            await fetch(`${API_BASE}/metrics/collect`, { method: "POST" });

            const latestRes = await fetch(`${API_BASE}/metrics/latest`);
            const historyRes = await fetch(`${API_BASE}/metrics/history?take=10`);

            if (!latestRes.ok || !historyRes.ok) {
                throw new Error("Error al obtener datos");
            }

            const latestData = await latestRes.json();
            const historyData = await historyRes.json();

            setData(latestData);
            setHistory(historyData);

            setLastUpdate(new Date().toLocaleTimeString());
        } catch (e) {
            setErr(e.message || "Error");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        load();
        const id = setInterval(load, 5000);
        return () => clearInterval(id);
    }, []);

    return (
        <div className="app">
            <div className="header">
                <h1 className="title">PROCESO</h1>
                <div className="subtitle">Monitor de recursos del sistema</div>
            </div>

            <div className="topbar">
                <div className="status">
                    {lastUpdate ? `Última actualización: ${lastUpdate}` : "Sin datos todavía"}
                </div>

                <div className="actions">
                    <button className="refresh-btn" onClick={load}>
                        Refrescar ahora
                    </button>
                </div>
            </div>

            {err && <div className="error">Error: {err}</div>}
            {loading && !data && <div className="loading">Cargando métricas...</div>}

            <div className="cards">
                <MetricCard
                    title="CPU"
                    value={`${data?.cpuPercent ?? "—"}%`}
                    percent={Number(data?.cpuPercent ?? 0)}
                />

                <MetricCard
                    title="Memoria"
                    value={`${data?.memPercent ?? "—"}%`}
                    subtitle={`${data?.memUsedMb ?? "—"} MB de ${data?.memTotalMb ?? "—"} MB`}
                    percent={Number(data?.memPercent ?? 0)}
                />

                <MetricCard
                    title="Disco"
                    value={`${data?.diskPercent ?? "—"}%`}
                    subtitle={`${data?.diskUsedGb ?? "—"} GB de ${data?.diskTotalGb ?? "—"} GB`}
                    percent={Number(data?.diskPercent ?? 0)}
                />
            </div>

            <h2 style={{ marginTop: 30 }}>Uso en el tiempo</h2>

            <div className="chart-card">
                <ResponsiveContainer width="100%" height={320}>
                    <LineChart data={chartData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="time" />
                        <YAxis domain={[0, 100]} />
                        <Tooltip />
                        <Line type="monotone" dataKey="cpu" stroke="#2563eb" strokeWidth={3} name="CPU %" />
                        <Line type="monotone" dataKey="memory" stroke="#16a34a" strokeWidth={3} name="RAM %" />
                        <Line type="monotone" dataKey="disk" stroke="#dc2626" strokeWidth={3} name="Disco %" />
                    </LineChart>
                </ResponsiveContainer>
            </div>

            <h2 style={{ marginTop: 30 }}>Historial</h2>

            <div className="table-container">
                <table className="metrics-table">
                    <thead>
                    <tr>
                        <th>Hora</th>
                        <th>CPU</th>
                        <th>RAM</th>
                        <th>Disco</th>
                    </tr>
                    </thead>
                    <tbody>
                    {history.map((m) => (
                        <tr key={m.id}>
                            <td>{new Date(m.createdAt).toLocaleTimeString()}</td>
                            <td className={getLevel(m.cpuPercent)}>{m.cpuPercent}%</td>
                            <td>{m.memPercent}%</td>
                            <td>{m.diskPercent}%</td>

                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>

            <details className="details">
                <summary>Ver JSON crudo</summary>
                <pre className="json-box">{JSON.stringify(data, null, 2)}</pre>
            </details>
        </div>
    );
}

function MetricCard({ title, value, subtitle, percent }) {
    return (
        <div className="metric-card">
            <div className="metric-title">{title}</div>
            <div className="metric-big">{value}</div>
            {subtitle && <div className="metric-small">{subtitle}</div>}

            <div className="progress">
                <div
                    className={`progress-bar ${getLevel(percent)}`}
                    style={{ width: `${Math.max(0, Math.min(percent, 100))}%` }}
                />
            </div>
        </div>
    );
}

function getLevel(value) {
    if (value < 50) return "low";
    if (value < 80) return "medium";
    return "high";
}