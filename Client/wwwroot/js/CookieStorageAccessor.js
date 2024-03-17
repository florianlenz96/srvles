export function get(key)
{
    const cookie = document.cookie;
    if (cookie === "")
    {
        return null;
    }
        
    return cookie.split('; ').find(row => row.startsWith(key)).split('=')[1];
}

export function set(key, value)
{
    document.cookie = `${key}=${value}`;
}