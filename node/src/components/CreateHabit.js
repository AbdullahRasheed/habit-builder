import Post from "../RESTRequest";
import BasicForm from "./BasicForm";
import './CreateHabit.css';

function CreateHabit(properties){
    const fields = [
        {
            "type": "text",
            "name": "name",
            "placeholder": "Name"
        },
        {
            "type": "text",
            "name": "description",
            "placeholder": "Description"
        }
    ];

    async function submit(formData){
        await Post("habit/createhabit", formData)
            .then(response => properties.onResponse(response))
            .catch(err => alert(err));
    }

    return (
        <div className="create-habit-form">
            <BasicForm inputs={fields} submitName="Create" onSubmit={submit} />
        </div>
    )
}

export default CreateHabit;