namespace DeviantArtFs

open System
  
type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
}