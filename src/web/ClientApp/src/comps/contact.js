import React, { Component, Fragment } from 'react';
import {
    validate,
    TextInput,
    TextAreaInput,
    EmailInput,
    CheckInput,
    SelectInput
} from '../utils/forms';
import * as Api from '../utils/api';

const ResultPane = (props) => {
    return (
        <div className="span6">
            <div className="well">
                <h4>&iexcl;Thanks for reaching out to us!</h4>
                We'll get back to you shortly.
            </div>
            &nbsp;
        </div>
    );
}

export default class Contact extends Component {
    
    model = {
        name: '',
        email: '',
        phone: '',
        message: ''
    }

    constructor(props) {
        super(props);
        
        this.state = {
            isProcessing: false,
            hasProcessed: false,
            formIsValid: false,
            formControls: {
                name: {
                    value: '',
                    placeholder: 'Nombre *',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true
                    }
                },
                email: {
                    value: '',
                    placeholder: 'E-mail *',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isEmail: true,
                        isRequired: true
                    }
                },
                phone: {
                    value: '',
                    placeholder: 'Teléfono',
                    valid: true,
                    touched: true,
                    validationRules: {}
                },
                message: {
                    value: '',
                    placeholder: '¿En que podemos ayudar hoy? *',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true
                    }
                }
            }
        };
    }

    componentDidMount = () => {
        // Not much to do by now in here.
    }

    changeHandler = (event) => {
        const name = event.target.name;
        const value = event.target.value;

        const updatedControls = {
            ...this.state.formControls
        };

        const updatedFormElement = {
            ...updatedControls[name]
        };

        updatedFormElement.value = value;
        updatedFormElement.touched = true;
        updatedFormElement.valid = validate(value, updatedFormElement.validationRules)

        updatedControls[name] = updatedFormElement;

        let formIsValid = true;
        for (let inputIdentifier in updatedControls) {
            formIsValid = updatedControls[inputIdentifier].valid
            if (!formIsValid) break;
        }
        
        this.setState({
            formIsValid: formIsValid,
            formControls: updatedControls
        });
    }

    formSubmitHandler = event => {
        event.preventDefault();
        
        let _self = this;
        _self.setState({isProcessing: true});
        
        this.model.name = this.state.formControls.name.value;
        this.model.email = this.state.formControls.email.value;
        this.model.phone = this.state.formControls.phone.value;
        this.model.message = this.state.formControls.message.value;
        
        $.when(Api.postContact(this.model))
        .then((data) => {
            _self.setState({isProcessing: false, hasProcessed: data.done});
        });
        
    }

    render () {
        return(
            !this.state.hasProcessed
            ?
            <form onSubmit={ this.formSubmitHandler } noValidate>
                <div className="row">
                    <div className="col-12">
                        <legend><span>Déjenos su mensaje</span></legend>
                    </div>
                    <div className="col-12 col-sm-6">
                        <TextInput
                            name="name"
                            placeholder={this.state.formControls.name.placeholder}
                            value={this.state.formControls.name.value}
                            onChange={ this.changeHandler }
                            touched={this.state.formControls.name.touched}
                            valid={this.state.formControls.name.valid}
                        />
                        <EmailInput
                            name="email"
                            placeholder={this.state.formControls.email.placeholder}
                            value={this.state.formControls.email.value}
                            onChange={ this.changeHandler }
                            touched={this.state.formControls.email.touched}
                            valid={this.state.formControls.email.valid}
                        />
                        <TextInput
                            name="phone"
                            placeholder={this.state.formControls.phone.placeholder}
                            value={this.state.formControls.phone.value}
                            onChange={ this.changeHandler }
                            touched={this.state.formControls.phone.touched}
                            valid={this.state.formControls.phone.valid}
                        />
                    </div>
                    <div className="col-12 col-sm-6">
                        <TextAreaInput
                            name="message"
                            placeholder={this.state.formControls.message.placeholder}
                            value={this.state.formControls.message.value}
                            onChange={ this.changeHandler }
                            touched={this.state.formControls.message.touched}
                            valid={this.state.formControls.message.valid}
                            rows={10}
                            cols={20}
                        />
                    </div>
                    
                    <button
                        className="btn btn-primary btn-block"
                        type="submit"
                        disabled={this.state.isProcessing || !this.state.formIsValid}>
                            {!this.state.isProcessing
                            ?'Enviar'
                            :<span>Procesando...&nbsp;<i className="fas fa-spinner fa-spin"></i></span>}
                    </button>
                    &nbsp;
                </div>
            </form>
            :<ResultPane />
        );
    }   
}