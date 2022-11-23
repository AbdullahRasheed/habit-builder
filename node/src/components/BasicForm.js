import React from "react";
import './BasicForm.css';
import '../styles/ControlStyles.css';

export default function BasicForm(properties){
    const target = {};
    const inputs = properties.inputs.map((input, i) => {
        Object.assign(target, {[input.name]: ""});
        return <input onChange={handleChange} key={i} className="BasicForm-input" {...input} />
    });

    const [formData, setFormData] = React.useState(target);

    function handleChange(event){
        setFormData(prev => {
            return {
                ...prev,
                [event.target.name]: event.target.value
            }
        });
    };

    return (
        <div className="BasicForm-form">
            {inputs}
            <button className="std-btn" onClick={() => properties.onSubmit(formData)}>{properties.submitName}</button>
        </div>
    )
}