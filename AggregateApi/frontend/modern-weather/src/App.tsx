import { useState } from 'react'
import './App.css'

const API_BASE = 'http://localhost:5159/api/Aggregate'

// --- Types ---

interface FormParams {
  date: string
  sortBy: string
  company: string
  country: string
  category: string
  url: string
}

interface WeatherLocation {
  name: string
  country: string
  localtime: string
}

interface WeatherCondition {
  text: string
  icon: string
}

interface WeatherCurrent {
  temp_c: number
  feelslike_c: number
  humidity: number
  wind_kph: number
  vis_km: number
  condition: WeatherCondition
}

interface WeatherData {
  location: WeatherLocation
  current: WeatherCurrent
}

interface NewsSource {
  name: string
}

interface NewsArticle {
  title: string
  description: string
  url: string
  urlToImage: string
  publishedAt: string
  source: NewsSource
}

interface NewsData {
  status: string
  totalResults: number
  articles: NewsArticle[]
}

interface TwitterData {
  html: string
  author_name: string
  author_url: string
}

interface FallbackData {
  Message: string
}

interface AggregateResponse {
  weatherApiData: WeatherData | FallbackData
  newsApiData: NewsData | FallbackData
  newsApiCategoryData: NewsData | FallbackData
  twitterData: TwitterData | FallbackData
}

// --- Type guards ---

const isFallback = (data: unknown): data is FallbackData =>
  typeof data === 'object' && data !== null && 'Message' in data

const isWeatherData = (data: unknown): data is WeatherData =>
  typeof data === 'object' && data !== null && 'current' in data

const isNewsData = (data: unknown): data is NewsData =>
  typeof data === 'object' && data !== null && 'articles' in data

const isTwitterData = (data: unknown): data is TwitterData =>
  typeof data === 'object' && data !== null && 'html' in data

// --- Default params ---

const defaultParams: FormParams = {
  date: new Date().toISOString().split('T')[0],
  sortBy: 'popularity',
  company: 'Apple',
  country: 'Greece',
  category: 'business',
  url: 'https://twitter.com/Interior/status/507185938620219395',
}

// --- Sub-components ---

function Spinner() {
  return (
    <div className="spinner-wrap">
      <div className="spinner" />
    </div>
  )
}

interface WeatherStatProps {
  icon: string
  label: string
  value: string | number | undefined
}

function WeatherStat({ icon, label, value }: WeatherStatProps) {
  return (
    <div className="weather-stat">
      <span className="stat-icon">{icon}</span>
      <div>
        <div className="stat-label">{label}</div>
        <div className="stat-value">{value ?? '—'}</div>
      </div>
    </div>
  )
}

interface WeatherCardProps {
  data: WeatherData | FallbackData | undefined
}

function WeatherCard({ data }: WeatherCardProps) {
  if (!data) return null
  if (isFallback(data)) return <FallbackCard message={data.Message} icon="🌧️" />
  if (!isWeatherData(data)) return null

  const { location: loc, current: cur } = data

  return (
    <div className="agg-card weather-card">
      <div className="agg-card-header">
        <span className="agg-card-icon">🌤️</span>
        <div>
          <div className="agg-card-title">Weather</div>
          <div className="agg-card-subtitle">{loc?.name}, {loc?.country}</div>
        </div>
        <div className="weather-temp">{cur?.temp_c}°C</div>
      </div>
      <div className="weather-grid">
        <WeatherStat icon="🌡️" label="Feels Like" value={`${cur?.feelslike_c}°C`} />
        <WeatherStat icon="💧" label="Humidity" value={`${cur?.humidity}%`} />
        <WeatherStat icon="💨" label="Wind" value={`${cur?.wind_kph} kph`} />
        <WeatherStat icon="👁️" label="Visibility" value={`${cur?.vis_km} km`} />
        <WeatherStat icon="☁️" label="Condition" value={cur?.condition?.text} />
        <WeatherStat icon="🕐" label="Local Time" value={loc?.localtime?.split(' ')[1]} />
      </div>
    </div>
  )
}

interface NewsCardProps {
  data: NewsData | FallbackData | undefined
  title: string
  icon: string
}

function NewsCard({ data, title, icon }: NewsCardProps) {
  if (!data) return null
  if (isFallback(data)) return <FallbackCard message={data.Message} icon="📰" />
  if (!isNewsData(data)) return null

  const articles = data.articles ?? []

  return (
    <div className="agg-card news-card">
      <div className="agg-card-header">
        <span className="agg-card-icon">{icon}</span>
        <div>
          <div className="agg-card-title">{title}</div>
          <div className="agg-card-subtitle">{articles.length} articles found</div>
        </div>
        <div className="agg-badge">{data.status}</div>
      </div>
      <div className="articles-list">
        {articles.length === 0 && <p className="agg-empty">No articles found.</p>}
        {articles.slice(0, 5).map((a, i) => (
          <a key={i} href={a.url} target="_blank" rel="noreferrer" className="article-item">
            {a.urlToImage && (
              <img
                src={a.urlToImage}
                alt=""
                className="article-thumb"
                onError={(e) => { (e.target as HTMLImageElement).style.display = 'none' }}
              />
            )}
            <div className="article-body">
              <div className="article-source">
                {a.source?.name} · {a.publishedAt?.slice(0, 10)}
              </div>
              <div className="article-title">{a.title}</div>
              <div className="article-desc">
                {a.description?.slice(0, 120)}{a.description?.length > 120 ? '…' : ''}
              </div>
            </div>
          </a>
        ))}
      </div>
    </div>
  )
}

interface TwitterCardProps {
  data: TwitterData | FallbackData | undefined
}

function TwitterCard({ data }: TwitterCardProps) {
  if (!data) return null
  if (isFallback(data)) return <FallbackCard message={data.Message} icon="🐦" />
  if (!isTwitterData(data)) return null

  return (
    <div className="agg-card twitter-card">
      <div className="agg-card-header">
        <span className="agg-card-icon">🐦</span>
        <div>
          <div className="agg-card-title">Twitter Embed</div>
          <div className="agg-card-subtitle">{data.author_name}</div>
        </div>
      </div>
      <div
        className="twitter-html"
        dangerouslySetInnerHTML={{ __html: data.html }}
      />
    </div>
  )
}

interface FallbackCardProps {
  message: string
  icon: string
}

function FallbackCard({ message, icon }: FallbackCardProps) {
  return (
    <div className="agg-card fallback-card">
      <span style={{ fontSize: '2rem' }}>{icon}</span>
      <p className="fallback-msg">{message}</p>
    </div>
  )
}

interface ErrorBannerProps {
  message: string
}

function ErrorBanner({ message }: ErrorBannerProps) {
  return (
    <div className="error-banner">
      <span>⚠️</span> {message}
    </div>
  )
}

// --- Main App ---

function App() {
  const [params, setParams] = useState<FormParams>(defaultParams)
  const [data, setData] = useState<AggregateResponse | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [submitted, setSubmitted] = useState(false)

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setParams((p) => ({ ...p, [e.target.name]: e.target.value }))
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)
    setData(null)
    setSubmitted(true)

    try {
      const query = new URLSearchParams({
        date: params.date,
        sortBy: params.sortBy,
        company: params.company,
        country: params.country,
        category: params.category,
        url: params.url,
      }).toString()

      const res = await fetch(`${API_BASE}?${query}`)
      if (!res.ok) throw new Error(`API error: ${res.status} ${res.statusText}`)
      const json: AggregateResponse = await res.json()
      setData(json)
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Unknown error occurred')
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <style>{`
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
          --agg-bg: #0b0f1a;
          --agg-surface: #111827;
          --agg-surface2: #1a2236;
          --agg-border: #1f2d45;
          --agg-accent: #38bdf8;
          --agg-accent2: #818cf8;
          --agg-text: #e2e8f0;
          --agg-muted: #64748b;
          --agg-success: #34d399;
          --agg-radius: 16px;
          --agg-shadow: 0 4px 32px rgba(0,0,0,0.4);
        }

        .agg-root {
          font-family: 'DM Sans', sans-serif;
          background: var(--agg-bg);
          color: var(--agg-text);
          min-height: 100vh;
          padding: 2rem 1.5rem;
          background-image:
            radial-gradient(ellipse at 20% 0%, rgba(56,189,248,0.07) 0%, transparent 60%),
            radial-gradient(ellipse at 80% 100%, rgba(129,140,248,0.07) 0%, transparent 60%);
        }

        .agg-inner { max-width: 1100px; margin: 0 auto; }

        .agg-header { text-align: center; margin-bottom: 2.5rem; }
        .agg-eyebrow {
          font-family: 'Syne', sans-serif;
          font-size: 0.75rem;
          letter-spacing: 0.2em;
          text-transform: uppercase;
          color: var(--agg-accent);
          margin-bottom: 0.5rem;
        }
        .agg-header h1 {
          font-family: 'Syne', sans-serif;
          font-size: clamp(2rem, 5vw, 3rem);
          font-weight: 800;
          background: linear-gradient(135deg, var(--agg-accent), var(--agg-accent2));
          -webkit-background-clip: text;
          -webkit-text-fill-color: transparent;
          line-height: 1.1;
        }
        .agg-header p { color: var(--agg-muted); font-size: 0.95rem; margin-top: 0.5rem; }

        .agg-form-card {
          background: var(--agg-surface);
          border: 1px solid var(--agg-border);
          border-radius: var(--agg-radius);
          padding: 1.75rem;
          margin-bottom: 2rem;
          box-shadow: var(--agg-shadow);
        }
        .agg-form-grid {
          display: grid;
          grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
          gap: 1rem;
          margin-bottom: 1.25rem;
        }
        .agg-form-group { display: flex; flex-direction: column; gap: 0.4rem; }
        .agg-form-label {
          font-size: 0.72rem;
          font-weight: 600;
          letter-spacing: 0.1em;
          text-transform: uppercase;
          color: var(--agg-muted);
        }
        .agg-form-input {
          background: var(--agg-surface2);
          border: 1px solid var(--agg-border);
          border-radius: 10px;
          color: var(--agg-text);
          font-family: 'DM Sans', sans-serif;
          font-size: 0.9rem;
          padding: 0.6rem 0.85rem;
          transition: border-color 0.2s, box-shadow 0.2s;
          outline: none;
          width: 100%;
        }
        .agg-form-input:focus {
          border-color: var(--agg-accent);
          box-shadow: 0 0 0 3px rgba(56,189,248,0.12);
        }
        .agg-url-group { grid-column: 1 / -1; }

        .agg-submit-btn {
          width: 100%;
          background: linear-gradient(135deg, var(--agg-accent), var(--agg-accent2));
          border: none;
          border-radius: 10px;
          color: #fff;
          cursor: pointer;
          font-family: 'Syne', sans-serif;
          font-size: 0.95rem;
          font-weight: 700;
          letter-spacing: 0.05em;
          padding: 0.85rem 1.5rem;
          transition: opacity 0.2s, transform 0.1s;
        }
        .agg-submit-btn:hover { opacity: 0.9; transform: translateY(-1px); }
        .agg-submit-btn:active { transform: translateY(0); }
        .agg-submit-btn:disabled { opacity: 0.5; cursor: not-allowed; transform: none; }

        .spinner-wrap { display: flex; justify-content: center; padding: 3rem; }
        .spinner {
          width: 40px; height: 40px;
          border: 3px solid var(--agg-border);
          border-top-color: var(--agg-accent);
          border-radius: 50%;
          animation: agg-spin 0.7s linear infinite;
        }
        @keyframes agg-spin { to { transform: rotate(360deg); } }

        .error-banner {
          background: rgba(239,68,68,0.1);
          border: 1px solid rgba(239,68,68,0.3);
          border-radius: 10px;
          color: #fca5a5;
          display: flex;
          align-items: center;
          gap: 0.5rem;
          font-size: 0.9rem;
          margin-bottom: 1.5rem;
          padding: 0.85rem 1rem;
        }

        .agg-results-grid {
          display: grid;
          grid-template-columns: repeat(auto-fill, minmax(480px, 1fr));
          gap: 1.25rem;
        }
        @media (max-width: 600px) { .agg-results-grid { grid-template-columns: 1fr; } }

        .agg-card {
          background: var(--agg-surface);
          border: 1px solid var(--agg-border);
          border-radius: var(--agg-radius);
          box-shadow: var(--agg-shadow);
          overflow: hidden;
          animation: agg-fadeUp 0.4s ease both;
        }
        @keyframes agg-fadeUp {
          from { opacity: 0; transform: translateY(16px); }
          to   { opacity: 1; transform: translateY(0); }
        }

        .agg-card-header {
          display: flex;
          align-items: center;
          gap: 0.85rem;
          padding: 1.1rem 1.25rem;
          border-bottom: 1px solid var(--agg-border);
          background: var(--agg-surface2);
        }
        .agg-card-icon { font-size: 1.5rem; flex-shrink: 0; }
        .agg-card-title { font-family: 'Syne', sans-serif; font-weight: 700; font-size: 1rem; }
        .agg-card-subtitle { font-size: 0.78rem; color: var(--agg-muted); margin-top: 2px; }
        .agg-badge {
          margin-left: auto;
          background: rgba(52,211,153,0.15);
          border: 1px solid rgba(52,211,153,0.3);
          border-radius: 20px;
          color: var(--agg-success);
          font-size: 0.7rem;
          font-weight: 600;
          letter-spacing: 0.05em;
          padding: 0.2rem 0.6rem;
          text-transform: uppercase;
          flex-shrink: 0;
        }

        .weather-temp {
          margin-left: auto;
          font-family: 'Syne', sans-serif;
          font-size: 2rem;
          font-weight: 800;
          color: var(--agg-accent);
          flex-shrink: 0;
        }
        .weather-grid { display: grid; grid-template-columns: repeat(3, 1fr); }
        .weather-stat {
          display: flex;
          align-items: center;
          gap: 0.6rem;
          padding: 0.85rem 1rem;
          border-right: 1px solid var(--agg-border);
          border-bottom: 1px solid var(--agg-border);
        }
        .weather-stat:nth-child(3n) { border-right: none; }
        .weather-stat:nth-last-child(-n+3) { border-bottom: none; }
        .stat-icon { font-size: 1.1rem; flex-shrink: 0; }
        .stat-label { font-size: 0.68rem; color: var(--agg-muted); text-transform: uppercase; letter-spacing: 0.05em; }
        .stat-value { font-size: 0.88rem; font-weight: 500; margin-top: 1px; }

        .articles-list { padding: 0.5rem 0; }
        .article-item {
          display: flex;
          gap: 0.85rem;
          padding: 0.85rem 1.25rem;
          text-decoration: none;
          border-bottom: 1px solid var(--agg-border);
          transition: background 0.15s;
          color: inherit;
        }
        .article-item:last-child { border-bottom: none; }
        .article-item:hover { background: var(--agg-surface2); }
        .article-thumb {
          width: 64px; height: 64px;
          border-radius: 8px;
          object-fit: cover;
          flex-shrink: 0;
          background: var(--agg-border);
        }
        .article-body { flex: 1; min-width: 0; }
        .article-source { font-size: 0.68rem; color: var(--agg-accent); font-weight: 600; text-transform: uppercase; letter-spacing: 0.05em; margin-bottom: 3px; }
        .article-title { font-size: 0.85rem; font-weight: 600; line-height: 1.35; margin-bottom: 3px; }
        .article-desc { font-size: 0.76rem; color: var(--agg-muted); line-height: 1.4; }
        .agg-empty { color: var(--agg-muted); font-size: 0.85rem; padding: 1.5rem 1.25rem; }

        .twitter-html { padding: 1.25rem; }
        .twitter-html blockquote { border-left: 3px solid var(--agg-accent); padding-left: 1rem; color: var(--agg-muted); font-style: italic; }

        .fallback-card {
          display: flex;
          flex-direction: column;
          align-items: center;
          justify-content: center;
          gap: 0.75rem;
          padding: 2rem;
          text-align: center;
        }
        .fallback-msg { color: var(--agg-muted); font-size: 0.88rem; }

        .agg-empty-state { text-align: center; padding: 4rem 2rem; color: var(--agg-muted); }
        .agg-empty-state-icon { font-size: 3rem; margin-bottom: 1rem; opacity: 0.4; }
        .agg-empty-state p { font-size: 0.95rem; }
      `}</style>

      <div className="agg-root">
        <div className="agg-inner">
          <header className="agg-header">
            <div className="agg-eyebrow">Aggregate Dashboard</div>
            <h1>Data Aggregator</h1>
            <p>Weather · News · Twitter — unified in one request</p>
          </header>

          <form className="agg-form-card" onSubmit={handleSubmit}>
            <div className="agg-form-grid">
              <div className="agg-form-group">
                <label className="agg-form-label">Date</label>
                <input className="agg-form-input" type="date" name="date" value={params.date} onChange={handleChange} required />
              </div>
              <div className="agg-form-group">
                <label className="agg-form-label">Company</label>
                <input className="agg-form-input" type="text" name="company" value={params.company} onChange={handleChange} placeholder="e.g. Apple" required />
              </div>
              <div className="agg-form-group">
                <label className="agg-form-label">Country</label>
                <input className="agg-form-input" type="text" name="country" value={params.country} onChange={handleChange} placeholder="e.g. Greece" required />
              </div>
              <div className="agg-form-group">
                <label className="agg-form-label">Sort By</label>
                <select className="agg-form-input" name="sortBy" value={params.sortBy} onChange={handleChange}>
                  <option value="popularity">Popularity</option>
                  <option value="relevancy">Relevancy</option>
                  <option value="publishedAt">Published At</option>
                </select>
              </div>
              <div className="agg-form-group">
                <label className="agg-form-label">Category</label>
                <select className="agg-form-input" name="category" value={params.category} onChange={handleChange}>
                  <option value="business">Business</option>
                  <option value="technology">Technology</option>
                  <option value="science">Science</option>
                  <option value="health">Health</option>
                  <option value="sports">Sports</option>
                  <option value="entertainment">Entertainment</option>
                  <option value="general">General</option>
                </select>
              </div>
              <div className="agg-form-group agg-url-group">
                <label className="agg-form-label">Twitter URL</label>
                <input className="agg-form-input" type="url" name="url" value={params.url} onChange={handleChange} placeholder="https://twitter.com/..." required />
              </div>
            </div>
            <button className="agg-submit-btn" type="submit" disabled={loading}>
              {loading ? 'Fetching Data…' : '🔍 Fetch Aggregate Data'}
            </button>
          </form>

          {error && <ErrorBanner message={error} />}
          {loading && <Spinner />}

          {!loading && !submitted && (
            <div className="agg-empty-state">
              <div className="agg-empty-state-icon">📡</div>
              <p>Fill in the form above and click <strong>Fetch Aggregate Data</strong> to get started.</p>
            </div>
          )}

          {!loading && data && (
            <div className="agg-results-grid">
              <WeatherCard data={data.weatherApiData as WeatherData | FallbackData} />
              <NewsCard data={data.newsApiData as NewsData | FallbackData} title="Top News" icon="📰" />
              <NewsCard data={data.newsApiCategoryData as NewsData | FallbackData} title="Category Headlines" icon="🗞️" />
              <TwitterCard data={data.twitterData as TwitterData | FallbackData} />
            </div>
          )}
        </div>
      </div>
    </>
  )
}

export default App
