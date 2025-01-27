var lorempicsumwallpaper = document.getElementById("lorempicsumwallpaper");
let currentResolution = "3840x2160"; // Default resolution

async function GetLoremPicsumWallpaper() {
    try {
        const [width, height] = currentResolution.split('x');
        const wallpaperUrl = `https://picsum.photos/${width}/${height}`;
        
        // Add timestamp to prevent caching
        lorempicsumwallpaper.src = `${wallpaperUrl}?t=${Date.now()}`;
        lorempicsumwallpaper.alt = "Lorem Picsum Wallpaper";
    } catch (error) {
        console.error("An error occurred while loading Lorem Picsum Wallpaper: ", error);
    }
}

function SucrosePropertyListener(name, val) {
    switch (name) {
        case "refreshInterval":
            // Update interval
            clearInterval(wallpaperInterval);
            wallpaperInterval = setInterval(GetLoremPicsumWallpaper, val.value * 60000);
            break;
        case "wallpaperResolution":
            // Update resolution
            currentResolution = val.items[val.value];
            GetLoremPicsumWallpaper();
            break;
    }
}

function SucroseStretchMode(Type) {
    switch (Type) {
        case "None":
            lorempicsumwallpaper.style.objectFit = "none";
            break;
        case "Fill":
            lorempicsumwallpaper.style.objectFit = "fill";
            break;
        case "Uniform":
            lorempicsumwallpaper.style.objectFit = "contain";
            break;
        case "UniformToFill":
            lorempicsumwallpaper.style.objectFit = "cover";
            break;
        default:
            break;
    }
}

window.addEventListener("load", GetLoremPicsumWallpaper);

// Initial interval (60 seconds)
let wallpaperInterval = setInterval(GetLoremPicsumWallpaper, 60000); 