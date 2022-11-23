import BasicForm from './components/BasicForm';
import './AuthPage.css';
import Post from './RESTRequest';
import axios from 'axios';

function SignupPage(){
    const fields = [
        {
            "type": "text",
            "name": "username",
            "placeholder": "Username"
        },
        {
            "type": "password",
            "name": "password",
            "placeholder": "Password"
        }
    ];

    async function submit(formData){
        await Post("auth/register", formData)
            .then(response => {
                localStorage.setItem('habitTrackerAccessTok', response.data);
                axios.defaults.headers.common['Authorization'] = "Bearer " + response.data;
            })
            .catch(err => alert(err));
    }

    return (
        <div className='form-page-content'>
            <h1>Sign up</h1>
            <BasicForm inputs={fields} submitName="Sign up" onSubmit={submit} />
        </div>
    )
}

export default SignupPage;