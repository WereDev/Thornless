import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as AncestryStore from '../../store/characterNameService';

import './AncestryList.scss';

// At runtime, Redux will merge together...
type AncestryProps =
  AncestryStore.CharacterNameState
  & typeof AncestryStore.actionCreators


class FetchData extends React.Component<AncestryProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  ancestryColumnRef = React.createRef<HTMLDivElement>();
  optionsColumnRef = React.createRef<HTMLDivElement>();
  namesColumnRef = React.createRef<HTMLDivElement>();

  public ancestrySelected(event: React.FormEvent<HTMLButtonElement>) {
    var selectedValue = event.currentTarget.value;
    this.props.setSelectedAncestry(selectedValue);
    this.props.requestAncestryOptions(() => {
      if (this.optionsColumnRef.current) {
        this.scrollToColumn(this.optionsColumnRef)
      }
    });

  }

  public optionSelected(event: React.FormEvent<HTMLButtonElement>) {
    var selectedOption = event.currentTarget.value;
    this.props.setSelectedOption(selectedOption);
    this.props.requestCharacterNames(() => {
      if (this.namesColumnRef.current) {
        this.scrollToColumn(this.namesColumnRef)
      }
    });
  }

  public goToTop(event: React.FormEvent<HTMLButtonElement>) {
    if (this.ancestryColumnRef.current) {
      this.scrollToColumn(this.ancestryColumnRef)
    }
  }

  public render() {
    return (
      <React.Fragment>
        <div id="ancestry-row">
          <div className="col-lg-4 col-12 col-content">{this.renderAncestyDropdown()}</div>
          <div className="col-lg-4 col-12 col-content">{this.renderOptionsDropdown()}</div>
          <div className="col-lg-4 col-12 col-content">{this.renderName()}</div>
        </div>
        {/* {this.renderCountDropdown()}
        {this.renderSubmitButton()} */}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAncestries();
  }

  private renderAncestyDropdown() {
    return (
      <div ref={this.ancestryColumnRef}>
        <div>
          <div>
            <h2>Ancestries</h2>
          </div>
        </div>
        <div>
          {this.props?.ancestries.sort((a, b) => a.sortOrder - b.sortOrder).map((ancestry: AncestryStore.Ancestry) =>
            <div className="select-button" key={ancestry.code}>
              <button value={ancestry.code} onClick={e => this.ancestrySelected(e)}>{ancestry.name}</button>
            </div>
          )}
        </div>
      </div>
    );
  }

  private renderOptionsDropdown() {
    return (
      <div ref={this.optionsColumnRef} className="flex-magic">
        <div className="d-flex flex-row">
          <div className="flex-grow-1">
            <h2>Options</h2>
          </div>
          <div className="flex-grow-1 align-self-center text-right">
            <button onClick={e => this.goToTop(e)} className="toTopLink">
              Back to Top
            </button>
          </div>
        </div>
        <div>
          <h3>{this.props?.ancestryOptions?.name}</h3>
        </div>
        {this.props?.ancestryOptions?.options.sort((a, b) => a.sortOrder - b.sortOrder).map((option: AncestryStore.NameCodeSort) =>
          <div className="select-button" key={option.code}>
            <button value={option.code} onClick={e => this.optionSelected(e)}>{option.name}</button>
          </div>
        )}
        <div>
          <p>{this.props?.ancestryOptions?.copyright}</p>
        </div>
      </div>
    );
  }

  private scrollToColumn(ref: React.RefObject<HTMLDivElement>) {
    if (ref?.current?.parentElement?.parentElement) {
      var offset = ref.current.parentElement.parentElement.offsetTop;
      var top = ref.current.parentElement.offsetTop;
      ref.current.parentElement.parentElement.scrollTo({
        top: top - offset,
        left: 0,
        behavior: 'smooth'
      });
    }
  }

  private renderName() {
    return (
      <div ref={this.namesColumnRef} className="flex-magic">
        <div className="d-flex flex-row">
          <div className="flex-grow-1">
            <h2>Names</h2>
          </div>
          <div className="flex-grow-1 align-self-center text-right">
            <button onClick={e => this.goToTop(e)} className="toTopLink">
              Back to Top
            </button>
          </div>
        </div>
        <div className="col-content">
          {this.props?.characterNames.map((name: AncestryStore.CharacterName) =>
            <div key={name.name}>
              <h3>{name.name}</h3>
              <h4>{name.ancestryName} | {name.optionName}</h4>
              <div>
                {name.definitions.map((definition: AncestryStore.CharacterNameDefinition) =>
                  <div key={name.name + '-' + definition.namePart}><b>{definition.namePart}:</b> {definition.meanings.join(", ")}</div>
                )}
              </div>
            </div>
          )}
        </div>
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.characterNames, // Selects which state properties are merged into the component's props
  AncestryStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
