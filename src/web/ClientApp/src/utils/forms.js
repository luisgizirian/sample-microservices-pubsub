import React, { Fragment } from 'react';

const validate = (value, rules, comparer) => {
    let isValid = true;
    
    for (let rule in rules) {
        switch (rule) {
            case 'minLength': 
                isValid = isValid && minLengthValidator(value, rules[rule]);
                break;
            case 'isRequired':
                isValid = isValid && requiredValidator(value);
                break;
            case 'isEmail':
                isValid = isValid && emailValidator(value);
                break;
            case 'passwordMatch':
                isValid = isValid && passwordMatchValidator(value, comparer);
                break;
            case 'passwordComplexity':
                isValid = isValid && passwordComplexityValidator(value);
                break;
            default:
                isValid = true;
        }
    }
    return isValid;
}

/**
 * minLength Val
 * @param  value 
 * @param  minLength
 * @return          
 */
const minLengthValidator = (value, minLength) => {
    return value.length >= minLength;
}

/**
 * Check to confirm that feild is required
 * 
 * @param  value 
 * @return       
 */
const requiredValidator = value => {
    return value.trim() !== '';	
}

/**
 * Email validation
 * 
 * @param value
 * @return 
 */
const emailValidator = value => {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(value).toLowerCase());
}

/**
 * Password validation
 * 
 * @param value
 * @return 
 */
const passwordMatchValidator = (value, comparer) => {
    return value == comparer;
}

/**
 * Password complexity validation
 * 
 * @param value
 * @return 
 */
const passwordComplexityValidator = (value) => {
    var re = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$/;
    return re.test(String(value));
}

const TextInput = (props) => {
    let formControl =
        (props.legacy !== "True")
            ?"form-control"
            :"input-block-level";
    
    let groupControlError = "";

    if (props.touched && !props.valid) {
        formControl += (props.legacy !== "True")
            ?" is-invalid"
            :" input-validation-error";
        groupControlError += (props.legacy !== "True")
            ?""
            :" error";
    }

    function getClearedProps(){
        if (props.label) {
            const {label, ...filteredProps } = props;
            return filteredProps;
        }
    }

    return (
        (props.legacy !== "True")
        ?(!props.label)
            ?<div className="form-group">
                <input type="text" className={formControl} {...props} />
            </div>
            :<div className="control-group">
                <label>{props.label}</label>
                <input type="text" className={formControl} {...getClearedProps()} />
            </div>
        :(!props.label)
        ?<div className={`control-group${groupControlError}`}>
            <input type="text" className={formControl} {...props} />
        </div>
        :<div className={`control-group${groupControlError}`}>
            <label>{props.label}</label>
            <input type="text" className={formControl} {...getClearedProps()} />
        </div>
    );
}

const TextAreaInput = (props) => {
    let formControl =
        (props.legacy !== "True")
            ?"form-control"
            :"input-block-level";
    
    let groupControlError = "";

    if (props.touched && !props.valid) {
        formControl += (props.legacy !== "True")
            ?" is-invalid"
            :" input-validation-error";
        groupControlError += (props.legacy !== "True")
            ?""
            :" error";
    }

    return (
        (props.legacy !== "True")
        ?<div className="form-group">
            <textarea className={formControl} {...props} />
        </div>
        :<div className={`control-group${groupControlError}`}>
            <textarea className={formControl} {...props} />
        </div>
    );
}

const EmailInput = (props) => {
    let formControl =
        (props.legacy !== "True")
            ?"form-control"
            :"input-block-level";
    
    let groupControlError = "";

    if (props.touched && !props.valid) {
        formControl += (props.legacy !== "True")
            ?" is-invalid"
            :" input-validation-error";
        groupControlError += (props.legacy !== "True")
            ?""
            :" error";
    }

    function getClearedProps(){
        if (props.label) {
            const {label, ...filteredProps } = props;
            return filteredProps;
        }
    }

    return (
        (props.legacy !== "True")
        ?(!props.label)
            ?<div className="form-group">
                <input type="email" className={formControl} {...props} />
            </div>
            :<div className="control-group">
                <label>{props.label}</label>
                <input type="email" className={formControl} {...getClearedProps()} />
            </div>
        :(!props.label)
        ?<div className={`control-group${groupControlError}`}>
            <input type="email" className={formControl} {...props} />
        </div>
        :<div className={`control-group${groupControlError}`}>
            <label>{props.label}</label>
            <input type="email" className={formControl} {...getClearedProps()} />
        </div>
    );
}

const PasswordInput = (props) => {
    let formControl =
        (props.legacy !== "True")
            ?"form-control"
            :"input-block-level";
    
    let groupControlError = "";

    if (props.touched && !props.valid) {
        formControl += (props.legacy !== "True")
            ?" is-invalid"
            :" input-validation-error";
        groupControlError += (props.legacy !== "True")
            ?""
            :" error";
    }

    return (
        (props.legacy !== "True")
        ?<div className="form-group">
            <input type="password" className={formControl} {...props} />
        </div>
        :<div className={`control-group${groupControlError}`}>
            <input type="password" className={formControl} {...props} />
        </div>
    );
}

const CheckInput = (props) => {
    return (
        <Fragment>
            <input type="checkbox" {...props} />&nbsp;
        </Fragment>
    );
}

export {
    validate,
    TextInput,
    TextAreaInput,
    EmailInput,
    PasswordInput,
    CheckInput
}