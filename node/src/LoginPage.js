import BasicForm from './components/BasicForm';
import './AuthPage.css';
import Post from './RESTRequest';

function LoginPage(){
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
        await Post("auth/login", formData)
            .catch(err => alert(err));
    }

    return (
        <div className='form-page-content'>
            <h1>Login</h1>
            <BasicForm inputs={fields} submitName="Login" onSubmit={submit} />
        </div>
    )
}

export default LoginPage;