import axios from 'axios';

async function Post(url, body, config=null){
    return await axios.post('https://localhost:7133/' + url, body, {withCredentials: true});
}

async function Get(url, config=null){
    return await axios.get('https://localhost:7133/' + url, {withCredentials: true});
}

export default Post;

export {
    Post,
    Get
}